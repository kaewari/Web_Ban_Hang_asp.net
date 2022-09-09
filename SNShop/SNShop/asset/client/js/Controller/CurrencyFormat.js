function FormatCurrency(money) {
    let raw = money.toString();
    let amt = "";
    for (let i = raw.length - 1, j = 1; i >= 0; i--, j++) {
        amt = raw[i] + amt;
        if (j % 3 == 0 && i != 0) { amt = "," + amt; }
    }
    return amt + " ₫";
}