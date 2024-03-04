var account =  {
    init: function () {
        account.registerEvents();
    },
    registerEvents: function() {
        $('.btnCancelOrder').off('click').on('click', function (e) {
            e.preventDefault();

            Swal.fire({
                title: 'Bạn có chắc muốn hủy đơn hàng?',
                //text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Có',
                cancelButtonText: 'Không',
                didOpen: () => {
                    // Điều chỉnh kích thước font chữ trong hộp thông báo
                    $('.swal2-actions').css('font-size', '12px');
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    var productId = parseInt($(this).data('id'));
                    account.cancelOrder(productId);
                }
            })
        });
    },
    cancelOrder: function (productID) {
        $.ajax({
            url: '/Account/OrderCancel',
            data: {
                id: productID
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    location.reload(true);
                }
                else {
                    alert('Lỗi');
                }
            }
        });
    }
}

account.init();
