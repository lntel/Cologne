<?php

namespace Cologne\Graphics\Read;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Graphics.php';

use \PDO;

use Cologne\Database as Database;
use Cologne\Graphics as Graphics;

$database = new Database;
$db = $database->connect();

$graphics = new Graphics($db);

$result = $graphics->read();

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