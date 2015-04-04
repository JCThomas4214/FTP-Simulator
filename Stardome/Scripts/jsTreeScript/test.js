

test("Perform test on checkVal", function () {   
    checkVal('C:\Users\Jason Lewis\Documents\Stardome\trunk\Stardome\Stardome\ABC/01 Demacia Rising.mp3');
    var tmp = [false, 0]
    var tmp2 = new Array;
    tmp2 = checkVal('C:\Users\Jason Lewis\Documents\Stardome\trunk\Stardome\Stardome\ABC/01 Demacia Rising.mp3')
    equals(tmp2[0], tmp[0], "the function should return false");
    equals(tmp2[1], tmp[1], "the function should return 0");

    insertItem(0, 'C:\Users\Jason Lewis\Documents\Stardome\trunk\Stardome\Stardome\ABC/01 Demacia Rising.mp3');
    tmp2 = checkVal('C:\Users\Jason Lewis\Documents\Stardome\trunk\Stardome\Stardome\ABC/01 Demacia Rising.mp3')
    equals(tmp2[0], !tmp[0], "the function should return true");
    equals(tmp2[1], tmp[1], "the function should return 0");
    deleteItem(0);
});


test("Perform test on insertItem", function () {    
    var tmp = fileList.length;    
    insertItem(0, 'C:\Users\Jason Lewis\Documents\Stardome\trunk\Stardome\Stardome\ABC/01 Demacia Rising.mp3');
    equals(fileList.length, tmp + 1, "FileList should increase in length");
    deleteItem(0);
});

test("Perform test on insertItem", function () {
    insertItem(0, 'C:\Users\Jason Lewis\Documents\Stardome\trunk\Stardome\Stardome\ABC/01 Demacia Rising.mp3');
    var tmp = fileList.length;
    deleteItem(0);
    equals(fileList.length, tmp - 1, "FileList should decrease in length");    
});

setTimeout(function () { test("Perform test on Dropdowns menu", function () {
    if (Role != 1) {        
        equals(htmlDDlist.length, 3 + subList.length, "htmlDDlist should be 3 + subList");
    }
});
 }, 2000);
