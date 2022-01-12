# Advent Of Code 2021

My solutions to the AoC 2021

https://adventofcode.com/2021

## Special Notes

### Day 8 Part 2

This was an interesting one, lets just say I put on Hot Fuzz the movie and started looking into how to break this down.  Finally worked it out when Nicholas Angle... Angel was riding into town on the horse!

I am sure there are better ways to work this out and I am sure there will be a huge number of people with elegant and clean solutions vs my janky code here but you know what it works!

The key was sorting the letters into order and inserting them into a table:

![Spreadsheet Doc](./Day8/spreadsheet.png)

Enabled me to find the other patterns such as the totals which gave me some easy wins.

The guide on the right in the screenshot shows how I got the segment codes.  It is a bit rough and ready but I hope it makes sense to those who have worked on the problem.

```plain
T = Top Segment
TR = Top Right Segment
TL = Top Left Segment
M = Middle Segment
BR = Bottom Right Segment
BL = Bottom Left Segment
B = Bottom Segment
```

### Day 14 Part 2

Exponential growth is fun! This one took a while to work out (probably shouldn't have) basically I was "growing" and generating the entire string at each step.

This works initially but the 40+ steps with exponential growth mean that it just isn't possible

Sat down with the old "pen and paper" routine and using what I learned in Day 11 where you just need to approach from a different angle which lead me to the pairings.

So we start by putting the starting template into pairs in a dictionary:

```plain
NNCB
```

| Pair | count |
| ---- | ----- |
| NN   | 1     |
| NC   | 1     |
| CB   | 1     |

We can then use it to generate the next "step" using first the insertion rules, Which gives us the following pairs:

```plain
NN -> C | NC & CN
NC -> B | NB & BC
CB -> H | CH & HB
```

Our new dictionary will look like this:

| Pair | count |
| ---- | ----- |
| NC   | 1     |
| CN   | 1     |
| NB   | 1     |
| BC   | 1     |
| CH   | 1     |
| HB   | 1     |

Repeat again with the new pairings

```plain
NC -> B | NB & BC
CN -> C | CC & CN
NB -> B | NB & BB
BC -> B | BB & BC
CH -> B | CB & BH
HB -> C | HC & CB
```

| Pair | count |
| ---- | ----- |
| NB   | 1     |
| BC   | 2     |
| CC   | 1     |
| CN   | 1     |
| NB   | 1     |
| BB   | 2     |
| CB   | 2     |
| BH   | 1     |
| HC   | 1     |

Repeat for n steps

Finally we add the first and last letter of the starting template as a pair, this is used later when breaking up the elements, in this example `NB`

To break it back into its elements we just add up the letters which gives us:

| Element | count |
| ------- | ----- |
| B       | 12    |
| C       | 8     |
| H       | 2     |
| N       | 4     |

Then divide by two:

| Element | count |
| ------- | ----- |
| B       | 6     |
| C       | 4     |
| H       | 1     |
| N       | 2     |

Subtract most common from least common we get:

6 (B) - 1 (H) = 5