Okay so I think I figured out some stuff mucking around on paper

1: If a number is equal to the length of the list, it won't move and needs to be ignored

2: Work from the end of the list backwards. A number that is itself positions away from the last item can be moved opposite value (*-1)

3. Deal with cycles by creating a linked list, popping, and insert at index? Might be fun to do a linked list thing that works