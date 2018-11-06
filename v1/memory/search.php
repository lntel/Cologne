<?php

namespace Cologne\Memory\Search;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Memory.php';

use \PDO;

use Cologne\Database as Database;
use Cologne\Memory as Memory;

$data = json_decode(file_get_contents('php://input'), true);

$database = new Database;
$db = $database->connect();

$memory = new Memory($db);

echo $data['key'];

$result = $memory->search($data['key']);

print_r($result);

$num = $result->rowCount();

echo $num;

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