Advertisting = read.table('C:\\Projects\\StatisticalLearning\\datasets\\Advertising.csv', header = TRUE, sep=",")
Advertisting$X <- NULL
attach(Advertisting)
plot(sales,TV)
AdvertisingLM = lm(sales~TV)
AdvertisingLM
summary(AdvertisingLM)
abline(AdvertisingLM)