document.addEventListener("DOMContentLoaded", function () {

    var toggleButtons =
        document.querySelectorAll(".reset-password-toggle");

    for (var i = 0; i < toggleButtons.length; i++) {

        var button = toggleButtons[i];

        button.addEventListener("click", function () {

            var targetId =
                this.getAttribute("data-password-target");

            if (!targetId) {
                return;
            }

            var input =
                document.getElementById(targetId);

            if (!input) {
                return;
            }

            if (input.type === "password") {

                input.type = "text";

                this.innerHTML = "🙈";
                this.setAttribute(
                    "title",
                    "Hide password"
                );

            }
            else {

                input.type = "password";

                this.innerHTML = "👁";
                this.setAttribute(
                    "title",
                    "Show password"
                );
            }
        });
    }
});