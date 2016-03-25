#include "psychvrlauncher.h"
#include "ui_psychvrlauncher.h"

PsychVRLauncher::PsychVRLauncher(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::PsychVRLauncher)
{
    ui->setupUi(this);
}

PsychVRLauncher::~PsychVRLauncher()
{
    delete ui;
}
