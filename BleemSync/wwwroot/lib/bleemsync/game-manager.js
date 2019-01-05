class GameManager {
    constructor() {
        this._tree = $('.game-manager-node-tree');
        this._forms = $('.game-manager-form');
        this._editGameForm = $('#edit-game-form');
        this._addGameForm = $('#add-game-form');

        this.Init();
    }

    Init() {
        $.getJSON('/Games/GetTree', (data) => this.InitTree(data));
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
}