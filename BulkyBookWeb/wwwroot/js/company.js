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
                "url":"/Admin/Company/GetAll"
            },
            "columns": [
                { "data": "name", "width": "15%" },
                { "data": "streetAddress", "width": "15%" },
                { "data": "city", "width": "15%" },
                { "data": "state", "width": "15%" },
                { "data": "phoneNumber", "width": "15%" },
                {
                    "data": "id",             // id - id of the product
                    "render": function(data) {    // data - id of the product selected
                        return `
                                < div class="w-75 btn-group" role="group" >
                                <a href="/Admin/Company/Upsert?id=${data}"
                                class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                                <a onclick=Delete('/Admin/Company/Delete/${data}')
                                class="btn btn-danger mx-2"> <i class="bi bi-trash"></i> Delete</a>
                                </div >
                               `
                    },
                    "width": "15%"
                }
            ]
        }
    );
}

function Delete(url)
{
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message)
                    }
                    else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}
