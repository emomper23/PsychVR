#include "settings.h"
#include "ui_settings.h"
#include<QDebug>
#include<QProcess>
//#include<poppler/qt5/poppler-qt5.h>
#include<QAbstractButton>

#include <QSlider>
#include <QButtonGroup>
#include <QFile>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonValue>
#include <QByteArray>
#include <QJsonArray>
#include <QSignalMapper>
#include <QColor>
#include <QColorDialog>
#include <QFileDialog>

settings::settings(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::settings)
{
    ui->setupUi(this);

    connect(ui->buttonBox, SIGNAL(accepted()), this, SLOT(changeSettings()));
    connect(ui->buttonBox, SIGNAL(accepted()), this, SLOT());
    connect(ui->colorBut1, SIGNAL(clicked(bool)), this, SLOT(changeColor()));
    connect(ui->colorBut2, SIGNAL(clicked(bool)), this, SLOT(changeColor()));
    connect(ui->colorBut3, SIGNAL(clicked(bool)), this, SLOT(changeColor()));
    connect(ui->fileButton, SIGNAL(clicked(bool)),this,SLOT(changeFile()));


    QColorDialog setSkin;
    QString colorList[] = {"#2D221E","#3C2E28","#4B3932","#695046","#785C50","#87675A","#967264","#A57E6E","#C39582","#D2A18C","#E1AC96","#F0B8A0","#FFC3AA","#FFCEB4","#FFDABE","#FFE5C8"};


    for(int arraySpot = 0; arraySpot < 16; arraySpot ++ )
    {
        setSkin.setCustomColor(arraySpot,QColor(colorList[arraySpot]));
    }
    usernum = 0;

    QPalette pal = ui->widget->palette();
    QColor curCol = QColor("#D2A18C");
    pal.setColor(QPalette::Window,curCol);

    ui->color1->setAutoFillBackground(true);
    ui->color2->setAutoFillBackground(true);
    ui->color3->setAutoFillBackground(true);

    ui->color1->setPalette( pal);
    ui->color1->update();
    ui->color2->setPalette( pal);
    ui->color2->update();
    ui->color3->setPalette( pal);
    ui->color3->update();

    QString powerpoint = "test.ppt";
    ui->fileLabel->setText(powerpoint);

    // ui->pushButton->repaint();


}

settings::~settings()
{
    delete ui;
}


void settings::changeSettings()
{

    QJsonArray tester;

    QString filename = QApplication::applicationDirPath() + "/save.json";
    QFile saveFile(filename);
    if (!saveFile.open(QIODevice::ReadOnly)) {
           qWarning("Failed to save data.");
           //return false; c
    }
    else
    {
        QByteArray saveData = saveFile.readAll();
        QJsonDocument loadDoc(QJsonDocument::fromJson(saveData));
        tester = loadDoc.array();
    }

    saveFile.close();

   // int usern = ui->userLabel->text().right(1).toInt();

    QJsonObject heightScene = tester.at(usernum).toObject()["Heights"].toObject();
    QJsonObject calmScene = tester.at(usernum).toObject()["Calm"].toObject();
    QJsonObject socialScene = tester.at(usernum).toObject()["Social"].toObject();
    QJsonObject user = tester.at(usernum).toObject();


    QJsonObject heightSettings
    {
        {"Color",curCol.name()},
        {"Day", 1},
        {"Building", ui->heightBox->currentIndex()}
    };

    if(ui->radioButton_2->isChecked())
    {
        heightSettings["Day"] = 0;
    }

    QJsonArray animations;

    if(ui->checkBox->isChecked())
    {
        animations.append(0);
    }
    if(ui->checkBox_2->isChecked())
    {
        animations.append(1);
    }
    if(ui->checkBox_3->isChecked())
    {
        animations.append(2);
    }
    if(ui->checkBox_4->isChecked())
    {
        animations.append(3);
    }
    if(ui->checkBox_5->isChecked())
    {
        animations.append(4);
    }



    QJsonObject socialSettings
    {
        {"Color",curCol.name()},
        {"Animations", animations},
        {"Number Students", ui->seatsBox->value()},
        {"Powerpoint", powerpoint}
    };

    QJsonObject calmSettings
    {
        {"Color",curCol.name()},
        {"Rock", ui->checkBox_6->isChecked()},
        {"Tree", ui->checkBox_7->isChecked()}
    };

    heightScene["Settings"] = heightSettings;
    socialScene["Settings"] = socialSettings;
    calmScene["Settings"] = calmSettings;

    user["Heights"] = heightScene;
    user["Calm"] = calmScene;
    user["Social"] = socialScene;

    tester[usernum] = user;

    if (!saveFile.open(QIODevice::WriteOnly)) {
           qWarning("Failed to save data.");
           //return false;
       }
    QJsonDocument saveDoc(tester);

    saveFile.write(saveDoc.toJson());

    saveFile.close();

    delete ui;

}

void settings::changeColor()
{
    curCol = setSkin.getColor();

    QPalette pal = ui->color1->palette();
    pal.setColor(QPalette::Window,curCol);


    ui->color1->setPalette( pal);
    ui->color1->update();
    ui->color2->setPalette( pal);
    ui->color2->update();
    ui->color3->setPalette( pal);
    ui->color3->update();

}

void settings::changeFile()
{
    powerpoint =  QFileDialog::getOpenFileName(this,tr("Choose Powerpoint"), "", tr("Image Files (*.png *.jpg *.bmp)"));
    QString label = powerpoint;
    label.remove(0,powerpoint.lastIndexOf("/") + 1);
    ui->fileLabel->setText(label);

}

void settings::setupSettings(int  uid)
{
    usernum = uid;

}
