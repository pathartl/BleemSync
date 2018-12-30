function ExtractSerialFromBinFile(file) {
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

    //var bufferLength = 4194304;
    var bufferLength = 32768;
    var reader = new FileReader();
    var position = 0;
    var blob = file.slice(position, position + bufferLength);

    var triggerCharacters = [
        'C'.charCodeAt(),
        'E'.charCodeAt(),
        'H'.charCodeAt(),
        'L'.charCodeAt(),
        'S'.charCodeAt()
    ];

    reader.onload = function (loadedEvent) {
        //console.log(loadedEvent.target.result);
        ParseForSerial(loadedEvent.target.result);
    };

    // Init the reader and load the first bytes
    reader.readAsArrayBuffer(blob);

    function StringToCharArray(input) {
        var charArray = [];

        for (let stringChar in input.split(''))
        {
            charArray.push(stringChar.charCodeAt());
        }

        return charArray;
    }

    function ParseForSerial(result = new ArrayBuffer()) {
        let buffer = new Uint8Array(result);
        let bufferStr = String.fromCharCode.apply(null, buffer);

        for (let prefix of serialNumberPrefixes) {
            var prefixIndex = bufferStr.indexOf(prefix);
            if (prefixIndex !== -1) {
                var dirtySerial = bufferStr.slice(prefixIndex, prefixIndex + 11);
                try {
                    serial = CleanSerial(dirtySerial);
                } catch (e) {}
                break;
            }
        }

        if (serial == '') {
            position += bufferLength;
            blob = file.slice(position, position + bufferLength);
            reader.abort();
            reader.readAsArrayBuffer(blob);
        } else {
            $(document).trigger('SerialParsed', [serial]);
        }
    }

    function CleanSerial(dirtySerial) {
        const regex = /([C|E|H|L|S][A-Z]{2,3}).?(\d{2,5}).?(\d+)/gm;
        m = regex.exec(dirtySerial);

        var cleanSerial = m[1] + '-';
        var groups = m.length - 1;

        for (var i = 2; i <= groups; i++) {
            cleanSerial += m[i];
        }

        return cleanSerial;
    }


}

