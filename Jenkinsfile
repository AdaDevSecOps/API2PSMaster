def githubRepo = 'https://github.com/AdaDevSecOps/API2PSMaster.git'
def githubBranch = 'main'

def dockerRepo = 'https://docker.io/jirayusamrit/adasoft.jenkin.api/'

pipeline
{
    agent any
    environment
    {
        dockerImage = ''
    }
    stages{
        stage("Git Clone")
        {
            steps
            {
                echo "========Cloning Git========"
                git url: githubRepo,
                    branch: githubBranch,
                    credentialsId:'adadev_creds'
            }
            post
            {
                success
                {
                    echo "========Cloning Git successfully========"
                }
                failure
                {
                    echo "========Cloning Git failed========"
                }
            }
        }

        stage('Docker Build')
        {
            steps
            {
                echo "========Docker Building========"
                script
                {
                    dockerImage = docker.build("api2psmaster:5.18003.1.1", "./API2PSMaster/")
                }
            }
            post
            {
                success
                {
                    echo "========Docker Building successfully========"
                }
                failure
                {
                    echo "========Docker Building failed========"
                }
            }
        }
    }
}