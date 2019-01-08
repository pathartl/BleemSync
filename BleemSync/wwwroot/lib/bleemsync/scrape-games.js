class ParserResponse {
    constructor() {
        this.Game = null;
        this.Valid = true;
        this.Message = "";
    }
}

onmessage = function (e) {
    let reader = new FileReaderSync();
    let files = e.data;
    let games = [];

    for (let file of files) {
        let extension = file.name.split('.').pop();

        switch (extension) {
            case 'cue':
                games.push(ParseCueFile(file, files));
        }
    }

    postMessage(games);

    function ParseCueFile(file, files) {
        // Need all files so we can verify all parts of the .cue exist
        let response = new ParserResponse();
        let regex = /FILE \"(.+)\" BINARY/gm;
        let binFiles = [];

        let reader = new FileReaderSync();
        let contents = reader.readAsText(file);

        while (match = regex.exec(contents)) {
            binFiles.push(match[1]);
        }

        if (binFiles.length == 0) {
            response.Valid = false;
            response.Message = "The cue sheet is incorrect or corrupted. No reference to binary files was found."
        } else {
            var binFilesFound = [];

            for (var binFile of binFiles) {
                var foundFile = false;

                for (let file of files) {
                    if (file.name == binFile && !foundFile) {
                        binFilesFound.push(file);
                        foundFile = true;
                    }
                }
            }

            if (binFilesFound.length === binFiles.length) {
                response.Valid = true;
                response.Game = GetGameInfoFromBinaryFiles(binFilesFound);
                response.Game.Files = [file];
                response.Game.Files.push(binFilesFound);
            } else {
                response.Valid = false;
                response.Message = "Not all binary files were selected, or they are misnamed."
            }
        }

        return response;
    }

    function GetGameInfoFromBinaryFiles(files) {
        var fingerprint = "";

        // Test PlayStation first, other parsers will go under here
        fingerprint = GetPlayStationFingerprint(files[0]);

        if (fingerprint != '') {
            return GetGameInfoFromCentral(fingerprint, 'PlayStation');
        }
    }

    function GetPlayStationFingerprint(file) {
        var serial = "";
        var foundPossibleString = false;
        var foundSerial = false;

        var serialNumberPrefixes = [
            "CPCS",
            "ESPM",
            "HPS",
            "LPS",
            "LSP",
            "SCAJ",
            "SCED",
            "SCES",
            "SCPS",
            "SCUS",
            "SIPS",
            "SLES",
            "SLKA",
            "SLPM",
            "SLPS",
            "SLUS"
        ];

        var bufferSize = 32768;
        var reader = new FileReaderSync();

        for (let position = 0; position < file.size; position += bufferSize) {
            let blob = file.slice(position, position + bufferSize);
            let buffer = new Uint8Array(reader.readAsArrayBuffer(blob));
            let bufferStr = String.fromCharCode.apply(null, buffer);

            for (let prefix of serialNumberPrefixes) {
                let prefixIndex = bufferStr.indexOf(prefix);

                if (prefixIndex !== -1) {
                    let dirtySerial = bufferStr.slice(prefixIndex, prefixIndex + 11);
                    try {
                        serial = CleanPlayStationSerial(dirtySerial);
                    } catch {}
                    break;
                }
            }

            if (serial != '') {
                break;
            }
        }

        return serial;
    }

    function CleanPlayStationSerial(dirtySerial) {
        const regex = /([C|E|H|L|S][A-Z]{2,3}).?(\d{2,5}).?(\d+)/gm;
        m = regex.exec(dirtySerial);

        var cleanSerial = m[1] + '-';
        var groups = m.length - 1;

        for (var i = 2; i <= groups; i++) {
            cleanSerial += m[i];
        }

        return cleanSerial;
    }

    function GetGameInfoFromCentral(fingerprint, system) {
        let request = new XMLHttpRequest();

        request.open('GET', `https://central.bleemsync.app/api/${system}/GetBySerial/${fingerprint}`, false);
        request.send(null);

        if (request.status === 200) {
            console.log(request.responseText);
            return JSON.parse(request.responseText);
        } else {
            throw new DOMException(`Could not scrape data for a game with the fingerprint of ${fingerprint} for the system ${system}.`);
        }
    }
};