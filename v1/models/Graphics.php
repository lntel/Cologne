<?php

namespace Cologne;

use \PDO;

class Graphics {
    private $conn;
    private $table = 'gpu';

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