class GameManager {
    constructor() {
        this._tree = $('.game-manager-node-tree');
        this._forms = $('.game-manager-form');
        this._editGameForm = $('#edit-game-form');
        this._addGameForm = $('#add-game-form');
        this._uploadInput = $('input[type="file"]');

        this.Init();
    }

    Init() {
        $.getJSON('/Games/GetTree', (data) => this.InitTree(data));

        this._uploadInput.change(() => this.ParseUploader());
        this.LoadAddGameForm();
    }

    InitTree(data) {
        this._tree.jstree({
            'core': {
                'data': data,
                'check_callback': true
            },
            'types': {
                '#': {
                    'valid_children': ['Folder', 'Game']
                },
                'Folder': {
                    'valid_children': ['Folder', 'Game']
                },
                'Game': {
                    'valid_children': []
                }
            },
            'plugins': [
                'dnd',
                'types'
            ]
        })
            .on('move_node.jstree', (e, data) => this.TreeOnMoveNode(data))
            .on('select_node.jstree', (e, data) => this.TreeOnSelectNode(data));
    }

    TreeOnMoveNode(data) {
        var treeData = this._tree.jstree(true).get_json(null, { 'flat': true });

        $.ajax({
            type: 'POST',
            url: '/Games/SaveTree',
            data: { Nodes: treeData },
            dataType: 'json'
        });
    }

    TreeOnSelectNode(data) {
        this.LoadEditGameForm(data.node.id);
    }

    LoadEditGameForm(id) {
        this._forms.hide();

        $.getJSON(`/Games/GetById/${id}`, (data) => this.EditGame(data));
    }

    EditGame(data) {
        this._editGameForm.show().setViewModel(data);
    }

    LoadAddGameForm(viewModel) {
        this._forms.hide();
        this._addGameForm.show().setViewModel(viewModel);
    }

    ReadFileAsText(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();

            reader.onload = function (event) {
                resolve(event.target.result);
            };

            reader.onerror = reject;

            reader.readAsText(file);
        });
    }

    ParseUploader() {
        var uploader = this._uploadInput.get(0);
        var worker = new Worker('/lib/bleemsync/scrape-games.js');

        worker.postMessage(uploader.files);

        var parsingPromises = [];

        for (let file of uploader.files) {
            switch (file.name.split('.').pop()) {
                case 'cue':
                    //parsingPromises.push(this.ParseCueSheet(file, uploader.files));
                    break;
            }
        }

        var gameResults = Promise.all(parsingPromises).then((games) => {
            console.log(games);
        });
    }

    ParseCueSheet(cueSheet, allFiles) {
        let contentPromise = this.ReadFileAsText(cueSheet);
        /*
        return new Promise((resolve, reject) => {
            var reader = new FileReader();
            
            reader.onerror = () => {
                reader.abort();
                reject(new DOMException("There was a problem parsing the cue sheet as a file."));
            };

            reader.onload = () => {
                var binFiles = [];

                var regex = /FILE \"(.+)\" BINARY/m;
                var matches = reader.result.match(regex);

                for (var match of matches) {
                    binFiles.push(match);
                }

                if (binFiles.length == 0) {
                    reject(new DOMException("No reference to any .bin files was found."));
                } else {
                    var binFilesFound = [];

                    for (var binFile of binFiles) {
                        var foundFile = false;

                        for (var file of allFiles) {
                            if (file.name == binFile && !foundFile) {
                                binFilesFound.push(file);
                                foundFile = true;
                            }
                        }
                    }

                    if (binFilesFound.length === binFiles.length) {
                        resolve(binFiles);
                    } else {
                        reject(new DOMException("You must upload all .bin files associated with the cue sheet."));
                    }
                }

                reader.readAsText(cueSheet);
            };
        });
        */
    }


}