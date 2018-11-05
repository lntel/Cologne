<?php

namespace Bonsai;

use \PDO;

class Processor {
    private $conn;
    private $table = 'processor';

    public $id;
    public $name;
    public $price;

    public function __construct($instance)
    {
        $this->conn = $instance;
    }

    public function read()
    {
        $query = 'SELECT
                id,
                name,
                price
            FROM
                ' . $this->table;
        
        $result = $this->conn->query($query);

        return $result;
    }
}

?>