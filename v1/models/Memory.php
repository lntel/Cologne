<?php

namespace Bonsai;

use \PDO;

class Memory {
    private $conn;
    private $table = 'memory';

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