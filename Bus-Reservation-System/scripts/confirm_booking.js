const payNowButton = document.getElementById('PayNow');
let selectedSeats = [];
const seatButtons = document.querySelectorAll('.seat');
const selectedSeatsDiv = document.getElementById('selectedSeats');

let booking_form = document.getElementById('booking_form');
let info_loader = document.getElementById('info_loader');
let pay_info = document.getElementById('pay_info');

const maxSeats = parseInt(booking_form.elements['passengers'].value);
let amount = 0;

seatButtons.forEach(button => {
    button.addEventListener('click', function () {
        const seatId = this.getAttribute('data-seat-id');
        const seatNumber = this.getAttribute('data-seat-number');
        const seatImage = this.querySelector('img');

        if (selectedSeats.some(seat => seat.seatId === seatId)) {
            selectedSeats = selectedSeats.filter(seat => seat.seatId !== seatId);
            this.classList.remove('selected');

            if (seatImage) {
                seatImage.src = 'images/seat.png';
            }
        } 
        else if (selectedSeats.length < maxSeats) {
            selectedSeats.push({ seatId, seatNumber });
            this.classList.add('selected');

            if (seatImage) {
                seatImage.src = 'images/book-seat.png';
            }
        } 
        else {
            alert(`You can only select ${maxSeats} seat(s).`);
        }

        selectedSeatsDiv.innerHTML = selectedSeats.map(seat => {
            return `<span class="badge badge-pill bg-light text-dark">Seat ${seat.seatNumber}</span>`;
        }).join('');

        updatePayButtonState();
        checkAvailability();
    });
});

function updatePayButtonState() {
    if (selectedSeats.length === maxSeats && amount > 0) {
        payNowButton.removeAttribute('disabled');
    } else {
        payNowButton.setAttribute('disabled', true);
    }
}

function checkAvailability() {
    let source_val = booking_form.elements['source'].value;
    let destination_val = booking_form.elements['destination'].value;
    let passengers_val = booking_form.elements['passengers'].value;
    let date = booking_form.elements['date'].value;
    let name = booking_form.elements['name'].value;
    let number = booking_form.elements['phonenum'].value;
    let email = booking_form.elements['email'].value;

    if (selectedSeats.length === 0) {
        amount = 0;
        pay_info.classList.remove('d-none');
        pay_info.classList.replace('text-dark', 'text-danger');
        pay_info.innerHTML = 'Please select seats.';
        updatePayButtonState();
        return;
    }

    let selectedSeatsString = selectedSeats.map(seat => seat.seatId + '-' + seat.seatNumber).join(',');

    if (
        source_val !== '' &&
        destination_val !== '' &&
        passengers_val !== '' &&
        date !== '' &&
        name !== '' &&
        number !== '' &&
        email !== ''
    ) {
        pay_info.classList.add('d-none');
        info_loader.classList.remove('d-none');

        let data = new FormData();
        data.append('check_availability', '1');
        data.append('name', name);
        data.append('source', source_val);
        data.append('destination', destination_val);
        data.append('passengers', passengers_val);
        data.append('date', date);
        data.append('number', number);
        data.append('email', email);
        data.append('selectedSeats', selectedSeatsString);

        let xhr = new XMLHttpRequest();
        xhr.open('POST', 'ajax/confirm_booking.php', true);

        xhr.onload = function () {
            try {
                let response = JSON.parse(this.responseText);

                if (response && response.payment) {
                    amount = response.payment;

                    pay_info.innerHTML = `Total Amount to Pay: ₹${response.payment}`;
                    pay_info.classList.replace('text-danger', 'text-dark');
                    pay_info.classList.remove('d-none');
                } else {
                    amount = 0;
                    pay_info.innerHTML = response.info || 'Availability check failed.';
                    pay_info.classList.replace('text-dark', 'text-danger');
                    pay_info.classList.remove('d-none');
                }
            } catch (error) {
                amount = 0;
                console.error(this.responseText);
                pay_info.innerHTML = 'Invalid server response from confirm_booking.php.';
                pay_info.classList.replace('text-dark', 'text-danger');
                pay_info.classList.remove('d-none');
            }

            info_loader.classList.add('d-none');
            updatePayButtonState();
        };

        xhr.send(data);
    }
}

payNowButton.addEventListener('click', function (e) {
    e.preventDefault();

    if (selectedSeats.length !== maxSeats) {
        alert(`Please select exactly ${maxSeats} seat(s).`);
        return;
    }

    if (!amount || amount <= 0) {
        alert('Invalid amount.');
        return;
    }

    let formData = new FormData();
    formData.append('action', 'bookOffline');
    formData.append('payAmount', amount);

    fetch('ajax/submitpayment.php', {
        method: 'POST',
        body: formData
    })
    .then(response => response.text())
    .then(data => {
        console.log(data);

        let result;

        try {
            result = JSON.parse(data);
        } catch (error) {
            alert('Invalid server response. Check submitpayment.php.');
            console.error(data);
            return;
        }

       if (result.res === 'success') {
    let successModal = new bootstrap.Modal(document.getElementById('bookingSuccessModal'));
    successModal.show();
     document.getElementById('successOkBtn').onclick = function () {
    window.location.href = 'bookings.php';
    };
   } else {
    alert(result.info || 'Booking failed.');
   }
    });
});