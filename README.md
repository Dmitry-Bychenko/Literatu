# Literatu

Literatų Street is a [pretty little lane](https://en.wikipedia.org/wiki/Literat%C5%B3_Street) in Vilnus. This .Net 5 library contains **character**, **string** and **regular expressions** algorithms. 

## Some examples:

### CSV Parsing

```c#
IEnumerable<string[]> values = File
  .ReadLines(@"c:\myData.csv")
  .ParseCsv(CommaSeparateValue.ExcelWithComments); 
```

### Edit Procedure

```c#
string from = "abracadabra";
string to = "alakazam";

var edit = from.ToEditProcedure(to, 
  i => 1, // every insertion is of price 1
  d => 1, // every delition is of price 1
  (a, b) => a == b ? 0 : 1); // edit is of price 0 when no change, otherwise is of price 1

Console.WriteLine(edit.EditDistance);
Console.Write(string.Join(Environment.NewLine(edit.EditSequence));
```

Outcome

```
7
Keep 'a'
Edit 'b' into 'l'
Delete 'r'
Keep 'a'
Edit 'c' into 'k'
Keep 'a'
Edit 'd' into 'z'
Keep 'a'
Edit 'b' into 'm'
Delete 'r'
Delete 'a'
```
