$.fn.setViewModel = function (model) {
    for (let key of Object.keys(model)) {
        this.find(`[name="${key}"]`).val(model[key]);
    }

    return this;
}

$.fn.clearForm = function () {
    this.find('[name]').val(null);

    return this;
}