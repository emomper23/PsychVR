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

settings::settings(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::settings)
{
    ui->setupUi(this);

    connect(ui->buttonBox, SIGNAL(accepted()), this, SLOT(changeSettings()));
    connect(ui->buttonBox, SIGNAL(accepted()), this, SLOT());
    QPalette pal = ui->widget->palette();
    pal.setColor(QPalette::Window,QColor("#FF0000"));
    ui->widget->setPalette( pal);
    ui->widget->setAutoFillBackground(true);
    ui->widget->update();
   // ui->pushButton->repaint();


}

settings::~settings()
{
    delete ui;
}


void settings::changeSettings()
{

    QString stringlist[] = {"2D221E","3C2E28","4B3932","5A453C","695046","785C50","87675A","967264","A57E6E","B48A78","C39582","D2A18C","E1AC96","F0B8A0","FFC3AA","FFCEB4","FFDABE","FFE5C8"};


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
    int usern = 1;

    QJsonObject heightScene = tester.at(usern).toObject()["heights"].toObject();
    QJsonObject calmScene = tester.at(usern).toObject()["calm"].toObject();
    QJsonObject socialScene = tester.at(usern).toObject()["social"].toObject();
    QJsonObject user = tester.at(usern).toObject();


    QJsonObject settings
    {
        //{"Color", stringlist[ui->comboBox->currentIndex()]},
        {"Day", 1}
    };

    if(ui->radioButton_2->isChecked())
    {
        settings["Day"] = 0;
    }


       /*
    if(ui->scene_selection->currentIndex() == 0)
    {
        heightScene["Settings"] = settings;
    }
    else if(ui->scene_selection->currentIndex() == 1)
    {
        calmScene["Settings"] = settings;
    }
    else
    {
        socialScene["Settings"] = settings;
    }
    */
    heightScene["Settings"] = settings;

    user["Heights"] = heightScene;
    user["Calm"] = calmScene;
    user["Social"] = socialScene;

    tester[usern] = user;

    if (!saveFile.open(QIODevice::WriteOnly)) {
           qWarning("Failed to save data.");
           //return false;
       }
    QJsonDocument saveDoc(tester);

    saveFile.write(saveDoc.toJson());

    saveFile.close();

    delete ui;

}

