class GameManager {
    constructor() {
        this._tree = $('.game-manager-node-tree');
        this._forms = $('.game-manager-form');
        this._editGameForm = $('#edit-game-form');
        this._addGameForm = $('#add-game-form');
        this._addGameButton = $('.add-game-button');
        this._uploadInput = $('input[name="Files"]');
        this._progressBar = $('#progress-bar-modal');
        this._coverDropZone = $('.cover-dropzone');
        this._coverInput = $('input[name="Cover"]');
        this._coverPreview = $('.cover-preview');

        this.Init();
    }

    Init() {
        $.getJSON('/Games/GetTree', (data) => this.InitTree(data));

        this._uploadInput.change(() => this.ParseUploader());

        this._addGameForm.on('GameAdded', () => {
            this.OnGameAdded();
        });

        this._editGameForm.on('GameUpdated', () => {
            this.OnGameUpdated();
        });

        this.LoadAddGameForm();

        $(document).on('Progress', (percentage) => {
            this.OnProgress(percentage);
        });

        this._addGameButton.on('click', (e) => {
            e.preventDefault();
            this.LoadAddGameForm();
        });

        this._coverDropZone.on('drop', (e) => {
            e.stopPropagation();
            e.preventDefault();
            this.CoverDropHandler(e.originalEvent);
        }).on('dragover', (e) => {
            e.stopPropagation();
            e.preventDefault();
            this._coverDropZone.addClass('dragging');
        }).on('dragend dragexit drop', (e) => {
            this._coverDropZone.removeClass('dragging');
        });
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

    TreeReload() {
        $.getJSON('/Games/GetTree', (data) => {
            this._tree.jstree(true).settings.core.data = data;
            this._tree.jstree(true).refresh();
        });
    }

    LoadEditGameForm(id) {
        this._forms.hide();

        $.getJSON(`/Games/GetById/${id}`, (data) => this.EditGame(data));
    }

    EditGame(data) {
        var d = new Date();
        this._editGameForm.show().setViewModel(data);
        this._editGameForm.find('.cover-preview').attr('src', `/Games/GetLocalCoverByGameId/${data.Id}?v=${d.getTime()}`);
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
        AlertService.Clear();
        var uploader = this._uploadInput.get(0);
        var worker = new Worker('/lib/bleemsync/scrape-games.js');

        worker.postMessage(uploader.files);

        worker.onmessage = (response) => {
            if (Array.isArray(response.data) && response.data.length > 0) {
                if (response.data[0].Valid) {
                    this.LoadAddGameForm(response.data[0].Game);
                    var d = new Date();
                    this._addGameForm.find('.cover-preview').attr('src', response.data[0].Game.CoverUrl);
                } else {
                    for (let error of response.data) {
                        AlertService.Error(error.Message);
                    }
                }
            }
        }
    }

    CoverDropHandler(e) {
        if (e.dataTransfer.types.includes('Files')) {
            var _url = window.URL || window.webkitURL;
            var files = e.dataTransfer.files;

            var input = $(e.target).find('input');
            var preview = $(e.target).find('.cover-preview');

            input.get(0).files = files;
            preview.attr('src', _url.createObjectURL(files[0]));
        }
    }

    OnGameUpdated() {
        this._progressBar.modal('hide');
        this.TreeReload();
        this._editGameForm.clearForm();
        this._forms.hide();
    }

    OnGameAdded() {
        this._progressBar.modal('hide');
        this._uploadInput.val(null);
        this.TreeReload();
        this._addGameForm.clearForm();
        this._forms.hide();
    }

}