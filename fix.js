function testReindex() {
  const name = "UrunSecenek[5].VarsayilanMi";
  const index = 2;
  const newName = name.replace(/UrunSecenek\[\d+\]/, `UrunSecenek[${index}]`);
  console.log(newName);
}
testReindex();
