<?php

namespace Cologne\Processor\Read;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Processor.php';

use \PDO;

use Cologne\Database as Database;
use Cologne\Processor as Processor;

$database = new Database;
$db = $database->connect();

$processor = new Processor($db);

$result = $processor->read();

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