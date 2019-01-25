$.fn.setViewModel = function (model) {
    try {
        for (let key of Object.keys(model)) {
            this.find(`[name="${key}"]`).val(model[key]);
        }
    } catch {}

    return this;
}

$.fn.clearForm = function () {
    this.find('[name]').not('button').val(null);

    return this;
}