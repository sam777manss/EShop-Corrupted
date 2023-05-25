// Select the target element
var targetEmail = document.querySelector('span[data-val-placeholder="m.Email"]');
var targetPassword = document.querySelector('span[data-val-placeholder="m.Password"]');
debugger;
// Create a new instance of MutationObserver
var observer = new MutationObserver(function () {

});
var Email = targetEmail.innerHTML;
var Password = targetEmail.innerHTML;

if (Email != "") {
    var input = $('input[name="email"]');
    input.attr('placeholder', targetEmail.innerHTML);
    input.addClass('text-danger');
    $('label[for="email"]').css('color', 'red');
}
if (Password != "") {
    var input = $('input[name="Password"]');
    input.attr('placeholder', targetPassword.innerHTML);
    input.addClass('text-danger');
    $('label[for="your_pass"]').css('color', 'red');
}
// Configuration options for the MutationObserver
var config = { childList: true, subtree: true };

// Start observing the target element
observer.observe(targetEmail, config);
observer.observe(targetPassword, config);