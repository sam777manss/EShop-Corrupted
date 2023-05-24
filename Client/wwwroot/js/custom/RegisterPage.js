// Select the target element
var targetName = document.querySelector('span[data-val-placeholder="m.Name"]');
var targetEmail = document.querySelector('span[data-val-placeholder="m.Email"]');
var targetPassword = document.querySelector('span[data-val-placeholder="m.Password"]');
var targetConfirmPassword = document.querySelector('span[data-val-placeholder="m.ConfirmPassword"]');

// Create a new instance of MutationObserver
var observer = new MutationObserver(function () {

});
var Name = targetName.innerHTML;
var Email = targetEmail.innerHTML;
var Password = targetEmail.innerHTML;
var ConfirmPassword = targetEmail.innerHTML;
if (Name != "") {
    var input = $('input[name="name"]');
    input.attr('placeholder', targetName.innerHTML);
    input.addClass('text-danger');
    $('label[for="name"]').css('color', 'red');
}
if (Email != "") {
    var input = $('input[name="email"]');
    input.attr('placeholder', targetEmail.innerHTML);
    input.addClass('text-danger');
    $('label[for="email"]').css('color', 'red');
}
if (Password != "") {
    var input = $('input[name="password"]');
    input.attr('placeholder', targetPassword.innerHTML);
    input.addClass('text-danger');
    $('label[for="pass"]').css('color', 'red');
}
if (ConfirmPassword != "") {
    var input = $('input[name="ConfirmPassword"]');
    input.attr('placeholder', targetConfirmPassword.innerHTML);
    input.addClass('text-danger');
    $('label[for="re-pass"]').css('color', 'red');
}
// Configuration options for the MutationObserver
var config = { childList: true, subtree: true };

// Start observing the target element
observer.observe(targetName, config);
observer.observe(targetEmail, config);
observer.observe(targetPassword, config);
observer.observe(targetConfirmPassword, config);
