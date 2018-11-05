<?php

namespace Bonsai\Student\Create;

include_once '../dependencies/database.php';
include_once '../models/Students.php';

use Bonsai\Database as Database;
use Bonsai\Student as Student;

use \PDO;

$database = new Database;
$db = $database->connect();

$student = new Student($db);

$data = json_decode(file_get_contents("php://input"), true);

$student->forename = $data['forename'];
$student->surname = $data['surname'];
$student->dob = $data['dob'];
$student->address = $data['address'];
$student->mobile = $data['mobile'];

$student->create();

?>