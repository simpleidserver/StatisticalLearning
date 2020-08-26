Logistic = read.table('C:\\Projects\\StatisticalLearning\\datasets\\Logistic.csv', header = TRUE, sep=",")
Logistic$X <- NULL
attach(Logistic)
model <- glm(canver~x1+x2,family=binomial(link='logit'),data=Logistic)

Binary = read.table('C:\\Projects\\StatisticalLearning\\datasets\\Binary.csv', header = TRUE, sep=",")
attach(Binary)
model <- glm(admit~gre+gpa,family=binomial(link='logit'),data=Binary)