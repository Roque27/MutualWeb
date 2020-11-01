$(function () {
    moment.locale('es');
});

function getQueryString() {
    var res = {};

    var parameterList = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    var parameterParts = null;
    var parameterValue = null;
    var len = parameterList.length;
    for (var i = 0; i < len; i++) {
        parameterParts = parameterList[i].split('=');

        res[decodeURIComponent(parameterParts[0])] =
            parameterParts.length == 2
            ? decodeURIComponent(parameterParts[1])
            : null;
    }

    return res;
}

function getDateTimePickerFormattedDate(inputId) {
    var dtpData = $('#' + inputId).data('DateTimePicker');
    return dtpData && dtpData.date && dtpData.date()
        ? dtpData.date().format('YYYY-MM-DD')
        : null;
}

function configDateTimePicker(selector) {
    $(selector || '.datepicker').datetimepicker({
        format: 'DD/MM/YYYY',
        useCurrent: false,
    });
}

function configBackButtons(selector) {
    $(selector || '.btn-back').on('click', function (ev) {
        ev.preventDefault();

        window.history.back();

        return false;
    });
}
