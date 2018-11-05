# Cologne
## What is Cologne?
Cologne is an API and software that uses big data regarding computer hardware to accurately estimate the original value of a given computer system.
## How the information was collected
Firstly I attempted scraping the data from https://uk.pcpartpicker.com/ however, I found that the site didn't have a large quanitiy of hardware therefore I moved onto https://www.cpubenchmark.net/ where I found the data was a lot easier to scrape and clean and the quanitiy of data was much larger opposed to the first website.

Code used to insert data to MySQL:
```php
<?php

namespace Bonsai;

include 'v1/dependencies/database.php';

class Scanner
{
	private $file;
	private $attributes;
	private $db;

	private $name;
	private $price;
	
	public function __construct($db, $name)
	{
		$this->file = fopen($name, 'r');
		$this->db = $db;
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

					echo $this->name . $this->price;

					$this->Insert($this->name, $this->price);
				break;
			}
			$x++;
		}
	}

	public function Insert($name, $price)
	{
		$query = $this->db->prepare('INSERT INTO gpu (name, price) VALUES (:name, :price)');
		$query->bindParam(':name', $name);
		$query->bindParam(':price', $price);

		$query->execute();
	}
}

$db = new Database;
$database = $db->connect();

$scan = new Scanner($database, 'gpu.txt');
$scan->scanAttributes();

?>
```
