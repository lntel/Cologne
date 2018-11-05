<?php

namespace Bonsai\Memory\Read;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Memory.php';

use \PDO;

use Bonsai\Database as Database;
use Bonsai\Memory as Memory;

$database = new Database;
$db = $database->connect();

$memory = new Memory($db);

$result = $memory->read();

$num = $result->rowCount();

if($num > 0)
{
    $arr = array();
    $arr['data'] = array();

    while($row = $result->fetch(PDO::FETCH_ASSOC))
    {
        extract($row);

        $tmp = array(
            'id' => $id,
            'name' => $name,
            'price' => $price
        );

        array_push($arr['data'], $tmp);
    }

    echo json_encode($arr);
}
?>