class GameManager {
    constructor() {
        this._tree = $('.game-manager-node-tree');
        this._forms = $('.game-manager-form');
        this._editGameForm = $('#edit-game-form');
        this._addGameForm = $('#add-game-form');
        this._uploadInput = $('input[type="file"]');
        this._progressBar = $('#progress-bar-modal');

        this.Init();
    }

    Init() {
        $.getJSON('/Games/GetTree', (data) => this.InitTree(data));

        this._uploadInput.change(() => this.ParseUploader());
        this._addGameForm.on('GameAdded', () => {
            this.OnGameAdded();
        });
        this.LoadAddGameForm();

        $(document).on('Progress', (percentage) => {
            this.OnProgress(percentage);
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

        worker.onmessage = (response) => {
            if (Array.isArray(response.data) && response.data.length > 0) {
                if (response.data[0].Valid) {
                    this.LoadAddGameForm(response.data[0].Game);
                }
            }
        }
    }

    OnGameAdded() {
        this._progressBar.modal('hide');
        this._uploadInput.val(null);
        this.TreeReload();
        this._addGameForm.clearForm();
        this._forms.hide();
    }

}