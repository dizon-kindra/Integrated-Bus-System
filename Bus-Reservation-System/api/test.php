<?php
require_once "db.php";

json_response([
    "success" => true,
    "message" => "API is working",
    "database" => "sr_db"
]);
?>