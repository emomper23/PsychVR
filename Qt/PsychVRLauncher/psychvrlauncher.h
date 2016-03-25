#ifndef PSYCHVRLAUNCHER_H
#define PSYCHVRLAUNCHER_H

#include <QMainWindow>

namespace Ui {
class PsychVRLauncher;
}

class PsychVRLauncher : public QMainWindow
{
    Q_OBJECT

public:
    explicit PsychVRLauncher(QWidget *parent = 0);
    ~PsychVRLauncher();

private:
    Ui::PsychVRLauncher *ui;
};

#endif // PSYCHVRLAUNCHER_H
