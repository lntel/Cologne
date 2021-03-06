<?php

namespace Cologne;

include 'v1/dependencies/database.php';

class Scanner
{
	private $file;
	private $attributes;
	private $db;

	private $name;
    private $price;
    private $table;
	
	public function __construct($db, $name, $table)
	{
		$this->file = fopen($name, 'r');
        $this->db = $db;
        $this->table = $table;
	}
	
	public function scanAttributes()
	{
		$i = 0;
		$x = 0;
		while($line = fgets($this->file))
		{
			switch($i)
			{
				case 0:
					$i++;

					$this->name = $line;
				break;
				
				case 1:
					//$this->attributes[$x]['rating'];
					$i++;
				break;
				
				case 2:
					$this->price = $line;
					$i = 0;

					$this->Insert($this->name, $this->price);
				break;
			}
			$x++;
		}
	}

	public function Insert($name, $price)
	{
        $i = 1;
		$query = $this->db->prepare('INSERT INTO ' . $this->table . ' (name, price) VALUES (:name, :price)');
		$query->bindParam(':name', $name);
		$query->bindParam(':price', $price);

        $query->execute();
        
        echo "Inserted record {$name}" . PHP_EOL; 
        $i++;
	}
}

$db = new Database;
$database = $db->connect(); // Initialisation and connection to the MySQL Database

$scan = new Scanner($database, $argv[1], $argv[2]); // Database instance and file name containing information
$scan->scanAttributes(); // subroutine includes sorting and insertion

?>