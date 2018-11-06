<?php

namespace Cologne;

use \PDO;

class Database
{
    public $pdo;

    public function connect()
    {
        try
        {
            $this->pdo = new PDO('mysql:host=localhost;dbname=cologne;charset=utf8', 'root', '');
        }
        catch(PDOException $e)
        {
            // TODO: Handle exception
        }

        return $this->pdo;
    }
}

?>