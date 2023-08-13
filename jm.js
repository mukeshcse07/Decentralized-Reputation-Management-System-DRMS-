const loginForm = document.querySelector(".login-form");
const registerForm = document.querySelector(".register-form");
const usernameInput = document.querySelector("#username");
const passwordInput = document.querySelector("#password");
const usernameRegisterInput = document.querySelector("#username-register");
const passwordRegisterInput = document.querySelector("#password-register");
const loginButton = document.querySelector("#login-button");
const registerButton = document.querySelector("#register-button");

loginButton.addEventListener("click", function() {
    const username = usernameInput.value;
    const password = passwordInput.value;

    // TODO: Implement login logic
});

registerButton.addEventListener("click", function() {
    const username = usernameRegisterInput.value;
    const password = passwordRegisterInput.value;

    // TODO: Implement registration logic
});
