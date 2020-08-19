Advertisting = read.table('C:\\Projects\\StatisticalLearning\\datasets\\Advertising.csv', header = TRUE, sep=",")
Advertisting$X <- NULL
attach(Advertisting)
plot(sales,TV)
AdvertisingLM = lm(sales~TV)
AdvertisingLM
summary(AdvertisingLM)
abline(AdvertisingLM)

StockPrice = read.table('C:\\Projects\\StatisticalLearning\\datasets\\StockPrice.csv', header = TRUE, sep=",")
StockPrice$i <- NULL
attach(StockPrice)
StockPriceLM = lm(StockIndexPrice~InterestRate+UnemploymentRate,data=StockPrice)