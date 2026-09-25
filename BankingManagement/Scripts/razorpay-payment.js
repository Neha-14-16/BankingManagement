document.addEventListener("DOMContentLoaded", function () {

    var button = document.getElementById("openRazorpayButton");

    if (!button) {
        return;
    }

    button.addEventListener("click", function () {

        if (typeof Razorpay === "undefined") {

            alert(
                "Razorpay Checkout could not be loaded. " +
                "Please check your internet connection."
            );

            return;
        }

        var key =
            button.getAttribute("data-key");

        var orderId =
            button.getAttribute("data-order-id");

        var amount =
            parseInt(
                button.getAttribute("data-amount"),
                10
            );

        var options = {

            key: key,

            amount: amount,

            currency: "INR",

            name: "Banking Management",

            description: "Test Payment",

            order_id: orderId,

            handler: function (response) {

                alert(
                    "Razorpay response received.\n\n" +
                    "Payment ID: " +
                    response.razorpay_payment_id
                );

            },

            modal: {

                ondismiss: function () {

                    console.log(
                        "Razorpay Checkout closed."
                    );

                }

            }

        };

        var razorpay =
            new Razorpay(options);

        razorpay.on(
            "payment.failed",
            function (response) {

                alert(
                    "Payment failed.\n\n" +
                    "Reason: " +
                    response.error.description
                );

            }
        );

        razorpay.open();

    });

});