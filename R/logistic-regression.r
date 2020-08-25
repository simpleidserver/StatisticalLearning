Logistic = read.table('C:\\Projects\\StatisticalLearning\\datasets\\Logistic.csv', header = TRUE, sep=",")
Logistic$X <- NULL
attach(Logistic)
model <- glm(canver~x1+x2,family=binomial(link='logit'),data=Logistic)