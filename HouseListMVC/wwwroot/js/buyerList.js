var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/houses/getall/",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [
            { "data": "address", "width": "20%" },
            { "data": "number", "width": "20%" },
            { "data": "age", "width": "20%" },
            { "data": "square", "width": "20%" },
            { "data": "description", "width": "20%" },
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}