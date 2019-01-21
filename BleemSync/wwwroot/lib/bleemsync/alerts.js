class AlertService {
    static Error(message) {
        var alert = $('<div />').attr('class', 'pmd-alert error').text(message);
        $('.pmd-alert-container.center.top').append(alert);
    }

    static Clear() {
        $('.pmd-alert-container').html('');
    }
}