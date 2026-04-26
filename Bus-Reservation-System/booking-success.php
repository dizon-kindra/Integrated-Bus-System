<?php

require('inc/links.php');
require('inc/header.php');

$order_id = isset($_GET['oid']) ? $_GET['oid'] : 'N/A';
?>

<div class="container my-5">
    <div class="row justify-content-center">
        <div class="col-lg-6 col-md-8">
            <div class="card border-0 shadow-lg rounded-4">
                <div class="card-body text-center p-5">

                    <div class="mb-3" style="font-size:70px;">✅</div>

                    <h2 class="fw-bold mb-3">Booking Successful!</h2>

                    <p class="text-muted mb-2">
                        Your seat has been reserved successfully.
                    </p>

                    <p class="mb-4">
                        Please pay at the terminal before departure.
                    </p>

                    <div class="alert alert-warning">
                        <strong>Order ID:</strong> <?php echo htmlspecialchars($order_id); ?>
                    </div>

                    <a href="bookings.php" class="btn text-white w-100 mb-2" style="background:#AD8B3A;">
                        View My Bookings
                    </a>

                    <a href="index.php" class="btn btn-outline-secondary w-100">
                        Back to Home
                    </a>

                </div>
            </div>
        </div>
    </div>
</div>

<?php require('inc/footer.php'); ?>