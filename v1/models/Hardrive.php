<?php

namespace Cologne;

class Hardrive {

    private $conn;
    private $table = 'hdd';

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

    public function search($key)
    {
        $query = "SELECT
                *
            FROM
                " . $this->table . "
            WHERE
                name LIKE ?";

        $sql = $this->conn->prepare($query);

        $key = "%" . $key . "%";

        $sql->bindParam(1, $key);
        $sql->execute();

        return $sql;
    }

}

?>