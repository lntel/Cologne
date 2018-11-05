<?php

namespace Bonsai;

use \PDO;

class Student {

    private $conn;
    private $table = "students";

    public $sid;
    public $forename;
    public $surname;
    public $dob;
    public $address;
    public $mobile;

    public function __construct($instance)
    {
        $this->conn = $instance;
    }

    public function read()
    {
        $query = 'SELECT
                sid,
                forename,
                surname,
                dob,
                address,
                mobile
            FROM
                ' . $this->table;
        
        $result = $this->conn->query($query);

        return $result;
    }

    public function create()
    {
        $query = 'INSERT INTO ' . $this->table . ' 
                    (
                        forename,
                        surname,
                        dob,
                        address,
                        mobile
                    )
                VALUES
                    (
                        :forename,
                        :surname,
                        :dob,
                        :address,
                        :mobile
                    )';
        
        $q = $this->conn->prepare($query);

        $q->bindParam(':forename', $this->forename);
        $q->bindParam(':surname', $this->surname);
        $q->bindParam(':dob', $this->dob);
        $q->bindParam(':address', $this->address);
        $q->bindParam(':mobile', $this->mobile);

        return $q->execute();
    }
}

?>