var checkOut = {
    init: function () {
        checkOut.loadProvince();
        checkOut.loadTotalMoneyOfCart();
        checkOut.registerEvent();
    },
    registerEvent: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var idProvince = $(this).val();

            if (idProvince != '') {
                checkOut.loadDistrict(parseInt(idProvince));
            }
            else {
                $('#ddlDistrict').html('');
                $('#ddlWard').html('');
            }

        });
        $('#ddlDistrict').off('change').on('change', function () {
            var idDistrict = $(this).val();
            var idProvince = $('#ddlProvince').val();

            if (idDistrict != '') {
                checkOut.loadWard(parseInt(idProvince), parseInt(idDistrict));
            }
            else {
                $('#ddlWard').html('');
            }

        });
        $('#btnCheckVoucher').off('click').on('click', function (e) {
            e.preventDefault();

            var discountCode = $('#txtDiscountCode').val();
            checkOut.checkVoucher(discountCode);
        });
        $('#txtDiscountCode').off('keyup').on('keyup', function () {
            $('#msgVoucher').html('');

        });
        $('#drTypePayment').off('click').on('click', function (e) {
            e.preventDefault();

            var type = $(this).val();
            $('#load_form_payment').hide();
            if (type == "2") {
                $('#load_form_payment').show();
            }
        });
    },
    loadProvince: function () {
        $.ajax({
            url: '/ShoppingCart/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value=""> -- Chọn Tỉnh / Thành -- </option>';
                    var data = res.data;

                    $.each(data, function (i, item) {
                        html += '<option value="' + item.id + '">' + item.name + '</option>';
                    });

                    $('#ddlProvince').html(html);
                }
            }
        });
    },
    loadDistrict: function (idProvince) {
        $.ajax({
            url: '/ShoppingCart/LoadDistrict',
            type: "POST",
            data: {
                provinceID: idProvince
            },
            dataType: "json",
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value=""> -- Chọn Quận / Huyện -- </option>';
                    var data = res.data;

                    $.each(data, function (i, item) {
                        html += '<option value="' + item.id + '">' + item.name + '</option>';
                    });

                    $('#ddlDistrict').html(html);
                }
            }
        });
    },
    loadWard: function (idProvince, idDistrict) {
        $.ajax({
            url: '/ShoppingCart/LoadWard',
            type: "POST",
            data: {
                provinceID: idProvince,
                districtID: idDistrict,
            },
            dataType: "json",
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value=""> -- Chọn Phường / Xã -- </option>';
                    var data = res.data;

                    $.each(data, function (i, item) {
                        html += '<option value="' + item.id + '">' + item.name + '</option>';
                    });

                    $('#ddlWard').html(html);
                }
            }
        });
    },
    loadTotalMoneyOfCart: function () {
        $.ajax({
            url: "/ShoppingCart/LoadTotalMoneyOfCart",
            type: "GET",
            dataType: "json",
            success: function (res) {
                var data = res.data;

                if (res.status == true) {
                    $('#txtTotalMoney').text(numeral(data).format('0,0') + '₫');

                    if (data <= 500000) {
                        $('#txtDeliveryFee').text(numeral(20000).format('0,0') + '₫');

                        $('#deliveryFee').val(20000);

                    }
                    else {
                        $('#txtDeliveryFee').text(numeral(0).format('0,0') + '₫');

                        $('#deliveryFee').val(0);
                    }

                    checkOut.loadTotalPayment();
                }
            }
        });
    },
    loadTotalPayment: function () {
        var txtTotalMoney = $('#txtTotalMoney').text();
        txtTotalMoney = txtTotalMoney.replace('₫', '').replace(/,/g, '');
        var totalMoney = parseFloat(txtTotalMoney);

        var txtDeliveryFee = $('#txtDeliveryFee').text();
        txtDeliveryFee = txtDeliveryFee.replace('₫', '').replace(/,/g, '');
        var deliveryFee = parseFloat(txtDeliveryFee);

        var txtVoucher = $('#txtVoucher').text();
        txtVoucher = txtVoucher.replace('₫', '').replace('- ','').replace(/,/g, '');
        var voucher = parseFloat(txtVoucher);

        var totalPayment = totalMoney + deliveryFee - voucher;
        $('#txtTotalPayment').text(numeral(totalPayment).format('0,0') + '₫');
    },
    checkVoucher: function (discountCode) {
        $.ajax({
            url: "/ShoppingCart/CheckVoucher",
            type: "POST",
            dataType: "json",
            data: {
                discountCode: discountCode,
            },
            success: function (res) {
                var data = res.data;

                if (res.status == true) {
                    var txtTotalMoney = $('#txtTotalMoney').text();
                    txtTotalMoney = txtTotalMoney.replace('₫', '');
                    txtTotalMoney = txtTotalMoney.replace(/,/g, '');

                    var totalMoney = parseFloat(txtTotalMoney);

                    const startDate = new Date(data.startDate);
                    const endDate = new Date(data.endDate);
                    var nowDate = new Date();

                    //console.log(startDate);
                    //console.log(endDate);
                    //console.log(nowDate);

                    if (nowDate < startDate) {
                        $('#msgVoucher').html('<span style="display: block; color: red; margin: 0 0 5px 15px">Chưa tới thời gian sử dụng</span >');

                        $('#txtVoucher').text('- 0₫');
                    }
                    else if (nowDate > endDate) {
                        $('#msgVoucher').html('<span style="display: block; color: red; margin: 0 0 5px 15px">Đã hết thời gian sử dụng</span >');

                        $('#txtVoucher').text('- 0₫');
                    }
                    else if (data.quantity == 0) {
                        $('#msgVoucher').html('<span style="display: block; color: red; margin: 0 0 5px 15px">Đã hết số lượng sử dụng</span >');

                        $('#txtVoucher').text('- 0₫');
                    }
                    else if (totalMoney < data.minimumValue) {
                        $('#msgVoucher').html('<span style="display: block; color: red; margin: 0 0 5px 15px">Chỉ áp dụng cho đơn hàng tối thiểu ' + numeral(data.minimumValue).format('0,0') + '₫' + '</span >');

                        $('#txtVoucher').text('- 0₫');
                    }
                    else {
                        $('#msgVoucher').html('<span style="display: block; color: green; margin: 0 0 5px 15px">Áp dụng mã giảm giá thành công</span>');

                        var discountValue = data.discountValue;
                        if (data.discountAmount == true) {
                            $('#txtVoucher').text('- ' + numeral(discountValue).format('0,0') + '₫');
                        }
                        else {
                            var temp = totalMoney * discountValue / 100;
                            $('#txtVoucher').text('- ' + numeral(temp).format('0,0') + '₫');
                        }

                        $('#voucherID').val(data.id);
                    }
                }
                else {
                    $('#msgVoucher').html('<span style="display: block; color: red; margin: 0 0 5px 15px">Mã giảm giá không hợp lệ</span>');

                    $('#txtVoucher').text('- 0₫');
                }

                checkOut.loadTotalPayment();
            }
        })
    },
}

checkOut.init();