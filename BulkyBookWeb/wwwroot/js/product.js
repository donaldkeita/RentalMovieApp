var dataTable;

$(document).ready(function () {
    loadDataTable();
});

// inside javaScript, we can not access asp controller and tag helpers (we don't need to include those here)

// tblData - id inside the index page
function loadDataTable()
{
    dataTable = $('#tblData').DataTable(
        {
            "ajax": {
                "url":"/Admin/Product/GetAll"
            },
            "columns": [
                { "data": "title", "width": "15%" },
                { "data": "isbn", "width": "15%" },
                { "data": "price", "width": "15%" },
                { "data": "author", "width": "15%" },
                { "data": "category.name", "width": "15%" },
                {
                    "data": "id",             // id - id of the product
                    "render": function(data) {
                        return `
                                < div class="w-75 btn-group" role = "group" >
                                <a href="Admin/Product/Upsert?id=${data}"
                                class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                                <a 
                                class="btn btn-danger mx-2"> <i class="bi bi-trash"></i> Delete</a>
                                </div >
                               `
                    },
                    "width": "15%"
                },
            ]
        }
    );
}
