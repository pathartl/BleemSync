$('input[type="file"]').change(function () {
    var file = this.files[0];
    ExtractSerialFromBinFile(file);
});

function TestExtractSerialFromInput() {
    var input = $('input[type="file"]').get(0);
    var file = input.files[0];
    ExtractSerialFromBinFile(file);
}

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
    var bufferLength = 128;
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

    function ParseForPossibleIndicesOfSerial(buffer) {
        var possibleIndices = [];

        for (let triggerCharacter in triggerCharacters) {
            // Search through the buffer for any possible trigger characters
            for (let i = 0; i < buffer.length; i++) {
                if (buffer[i] == triggerCharacter) {
                    foundPossibleString = true;
                    // Keep track of indexes of possible starts of serial
                    possibleIndices.push(i);
                }
            }
        }

        return possibleIndices
    }

    function ParseForSerialFromPossible(buffer, possibleIndices) {
        var serial = '';

        // Loop through each possible start of serial
        for (let i = 0; i < possibleIndices.length; i++) {
            // Truncate to save memory
            let truncatedBuffer = buffer.slice(possibleIndices[i]);
            let str = String.fromCharCode.apply(null, buffer);

            for (let prefix of serialNumberPrefixes) {
                // Check if starts with any prefix
                if (str.startsWith(prefix)) {
                    serial = CleanSerial(str.toUpperCase());

                    // Break out of the loop, cause we have the serial
                    i = possibleIndices.length;
                }
            }
        }

        return serial;
    }

    function ParseSerialFromBeginningOfBuffer(buffer) {
        let serial = '';
        let truncatedBuffer = buffer.slice(0, 11); // Truncate at 11 bytes, because that's what the PS max serial length is
        let bufferStr = String.fromCharCode.apply(null, buffer);


        for (let prefix of serialNumberPrefixes) {
            // Check if starts with any prefix
            if (bufferStr.startsWith(prefix)) {
                serial = CleanSerial(bufferStr.toUpperCase());
                break;
            }
        }

        return serial;
    }

    function BufferStartsWithTriggerCharacter(buffer) {
        var begins = false;

        for (let i = 0; i < triggerCharacters.length; i++) {
            if (buffer[0] == triggerCharacters[i]) {
                begins = true;
                i = triggerCharacters.length;
            }
        }

        return begins;
    }

    function ParseForSerial(result = new ArrayBuffer()) {
        let buffer = new Uint8Array(result);

        if (BufferStartsWithTriggerCharacter(buffer)) {
            serial = ParseForPossibleIndicesOfSerial(buffer);
        }

        if (serial !== '') {
            var possibleIndices = ParseForPossibleIndicesOfSerial(buffer);

            if (possibleIndices.length > 0) {
                position += possibleIndices[0];
                blob = file.slice(position, position + );
            }
        }

        if (possibleIndices.length > 0) {
            serial = ParseForSerialFromPossible(buffer, possibleIndices);
        }

        if (serial == '') {
            reader.abort();
            reader.readAsArrayBuffer(blob);
        }







        if (!foundPossibleString && !foundSomething) {
            var doneSearchingForPossible = false;

            ParseForPossibles(buffer);

            for (let i = 0; i < buffer.length; i++) {
                if (triggerCharacters.includes(buffer[i]) && !doneSearchingForPossible) {
                    doneSearchingForPossible = true;
                    foundPossibleString = true;
                    foundSomething = true;

                    ParseForSerialFromPossible(buffer)

                    position += i;
                    blob = file.slice(position, position + bufferLength);

                    // Easy jump out of the loop
                    i = buffer.length;
                }
            }

            reader.abort();
            reader.readAsArrayBuffer(blob);
        }

        if (foundPossibleString && !foundSerial && !foundSomething) {

        }

        if (!foundPossibleString && !foundSerial && !foundSomething) {
            position += bufferLength;
            blob = file.slice(position, position + bufferLength);
            reader.abort();
            reader.readAsArrayBuffer(blob);
        }

        if (foundSerial) {
            alert("Found serial " + serial);
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

