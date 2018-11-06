<?php

namespace Cologne\Graphics\Search;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Graphics.php';

use \PDO;

use Cologne\Database as Database;
use Cologne\Graphics as Graphics;

$data = json_decode(file_get_contents('php://input'), true);

$database = new Database;
$db = $database->connect();

$graphics = new Graphics($db);

$result = $graphics->search($data['key']);

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