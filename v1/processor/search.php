<?php

namespace Cologne\Processor\Search;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Processor.php';

use \PDO;

use Cologne\Database as Database;
use Cologne\Processor as Processor;

$data = json_decode(file_get_contents('php://input'), true);

$database = new Database;
$db = $database->connect();

$processor = new Processor($db);

echo $data['key'];

$result = $processor->search($data['key']);

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