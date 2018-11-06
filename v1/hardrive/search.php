<?php

namespace Cologne\Hardrive\Search;

header("Content-Type: application/json; charset=UTF-8");

include_once '../dependencies/database.php';
include_once '../models/Hardrive.php';

use \PDO;

use Cologne\Database as Database;
use Cologne\Hardrive as Hardrive;

$data = json_decode(file_get_contents('php://input'), true);

$database = new Database;
$db = $database->connect();

$hardrive = new Hardrive($db);

$result = $hardrive->search($data['key']);

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
} else {
    echo "{ 'Message': 'No records were found' }";
}
?>