<?php

namespace Bonsai\Student\Read;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Students.php';

use \PDO;

use Bonsai\Database as Database;
use Bonsai\Student as Student;

$database = new Database;
$db = $database->connect();

$student = new Student($db);

$result = $student->read();

$num = $result->rowCount();

if($num > 0)
{
    $arr = array();
    $arr['data'] = array();

    while($row = $result->fetch(PDO::FETCH_ASSOC))
    {
        extract($row);

        $tmp = array(
            'sid' => $sid,
            'forename' => $forename,
            'surname' => $surname,
            'dob' => $dob,
            'address' => $address,
            'mobile' => $mobile
        );

        array_push($arr['data'], $tmp);
    }

    echo json_encode($arr);
}
?>